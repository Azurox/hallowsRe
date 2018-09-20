import uuid from "uuid/v4";
import shuffle from "shuffle-array";
import { EventEmitter } from "events";
import Fighter from "./Fighter";
import { IMap } from "../../Schema/Map";
import Position from "../RelationalObject/Position";
import HumanFighter from "./HumanFighter";
import { ISpell } from "../../Schema/Spell";
import SpellProcessor from "./SpellProcessor";
import SpellImpact from "./SpellImpact";
import CheckinManager from "./CheckinManager";
import FightEndProcessor from "./FightEndProcessor";
import MonsterFighter from "../Fight/MonsterFighter";
import AIProcessor from "./AIProcessor";
import AICommand from "./AICommand";

export default class FightRework extends EventEmitter {
  /* CONST */
  PREPARATION_TIME: number = 90;
  TURN_TIME: number = 30;
  MAX_MEMBER_BY_SIDE: number = 5;

  id: string;
  io: SocketIO.Server;
  clock: number = 0;
  lockClock: boolean = false;
  phase: number = 0;
  started: boolean = false;
  blueTeam: Fighter[];
  redTeam: Fighter[];
  fightOrder: Fighter[];
  acceptedId: string;
  map: IMap;
  mapObstacles: Position[];
  placementCells: { position: Position; taken: boolean; side: Side }[];
  winningSide: Side;
  checkinManager = new CheckinManager();

  constructor(io: SocketIO.Server, blueTeam: Fighter[], redTeam: Fighter[], map: IMap) {
    super();
    this.io = io;
    this.blueTeam = blueTeam;
    this.redTeam = redTeam;
    this.map = map;
    this.initFight();
  }

  initFight() {
    // Initialize fight ID.
    this.id = uuid();

    // Initialize placement cells
    this.placementCells = this.map.placementCells.map(cell => {
      return { position: cell.position, taken: false, side: cell.color };
    });

    this.mapObstacles = this.map.getObstacles(); // Get all obstacles of the current map.
    shuffle(this.placementCells); // Allow to have player placed in a random way.
    this.fightOrder = this.blueTeam.concat(this.redTeam).sort((a, b) => b.speed - a.speed); // contains all player sorted by speed.

    // Place fighter on the first available placement cell.
    for (let i = 0; i < this.fightOrder.length; i++) {
      for (let j = 0; j < this.placementCells.length; j++) {
        if (!this.placementCells[j].taken && this.placementCells[j].side == this.fightOrder[i].side) {
          this.fightOrder[i].position = this.placementCells[j].position;
          this.placementCells[j].taken = true;
          break;
        }
      }
    }
  }

  /* Send the fight to every human player */
  startFight() {
    const humans = this.getTotalHumansFighter();
    for (const human of humans) {
      this.io.sockets.connected[human.getSocketId()].join(this.id);
      this.io.to(human.getSocketId()).emit("fightStarted", {
        id: this.id,
        placementCells: this.placementCells,
        fighters: this.fightOrder.map((fighter: Fighter) => {
          return {
            isMainPlayer: fighter.getId() == human.getId(),
            id: fighter.getId(),
            name: fighter.getName(),
            position: fighter.position,
            side: fighter.side,
            life: fighter.life,
            currentLife: fighter.currentLife,
            speed: fighter.speed,
            armor: fighter.armor,
            magicResistance: fighter.magicResistance,
            attackDamage: fighter.attackDamage,
            movementPoint: fighter.movementPoint,
            actionPoint: fighter.actionPoint,
            spells: fighter.getId() == human.getId() ? human.getSpells() : undefined
          };
        })
      });
    }
    this.started = true;
  }

  teleportPlayerPhase0(fighter: Fighter, position: Position) {
    if (this.phase != 0) return;
    for (const targetCell of this.placementCells) {
      if (targetCell.position.equals(position) && !targetCell.taken) {
        for (const oldCell of this.placementCells) {
          if (oldCell.position.equals(position)) {
            oldCell.taken = false;
          }
        }
        fighter.position = position;
        targetCell.taken = false;
        this.io.to(this.id).emit("teleportPreFight", { position: position, playerId: fighter.getId() });
      }
    }
  }

  setFighterReady(id: string) {
    const fighter = this.retrieveHumanFighterFromPlayerId(id);
    fighter.ready = true;
    if (this.checkEveryFighterReady()) {
      this.startFightPhase1();
    } else {
      this.io.to(this.id).emit("setReady", { playerId: fighter.getId() });
    }
  }

  startFightPhase1(timeout: boolean = false) {
    this.clock = 0;
    if (timeout) {
      this.phase = 1;
      for (let i = 0; i < this.fightOrder.length; i++) {
        this.fightOrder[i].ready = true;
      }
    }
    this.acceptedId = this.fightOrder[0].getId();
    this.io.to(this.id).emit("fightPhase1", { playerId: this.acceptedId });
  }

  checkEveryFighterReady(): boolean {
    const humans = this.getTotalHumansFighter();
    for (const human of humans) {
      if (!human.ready) return false;
    }
    return true;
  }

  getTotalHumansFighter(): HumanFighter[] {
    const allPlayers = this.blueTeam.concat(this.redTeam);
    const humans: HumanFighter[] = [];
    for (let i = 0; i < allPlayers.length; i++) {
      if (allPlayers[i].isRealPlayer) humans.push(<HumanFighter>allPlayers[i]);
    }
    return humans;
  }

  getFightersPosition(): Position[] {
    const positions: Position[] = [];
    for (const fighter of this.fightOrder) {
      positions.push(fighter.position);
    }
    return positions;
  }

  retrieveHumanFighterFromPlayerId(id: string): HumanFighter {
    const humans = this.getTotalHumansFighter();
    for (const human of humans) {
      if (human.getId() == id) return human;
    }
    throw Error("Fighter not found");
  }

  retrieveHumanFighterFromSocketId(id: string): HumanFighter {
    const humans = this.getTotalHumansFighter();
    for (const human of humans) {
      if (human.getSocketId() == id) return human;
    }
    throw Error("Fighter not found");
  }

  retrieveFighterFromPlayerId(id: string): Fighter {
    for (const fighter of this.fightOrder) {
      if (fighter.getId() == id) return fighter;
    }
    throw Error("Fighter not found");
  }

  moveHumanFighter(id: string, path: Position[]) {
    if (!this.checkPlayerTurn(id)) return new Error("Not player turn !");
    const currentFighter = this.retrieveHumanFighterFromPlayerId(id);
    let canMove = false;
    for (const fighter of this.fightOrder) {
      for (let i = 0; i < path.length; i++) {
        if (fighter.position.equals(path[i]) && fighter.getId() != id) {
          canMove = false;
        }
      }
    }

    if (path.length > currentFighter.currentMovementPoint) canMove = false;

    for (let i = 0; i < path.length; i++) {
      const appliedMove = this.applyCell(currentFighter, path[i]);
      if (appliedMove) {
        currentFighter.currentMovementPoint--;
        currentFighter.position = path[i];
      }
    }

    if (canMove) {
      this.io.to(this.id).emit("fighterMove", { playerId: id, path: path });
    }
  }

  moveMonsterFighter(monster: MonsterFighter, path: Position[]) {
    for (let i = 0; i < path.length; i++) {
      const appliedMove = this.applyCell(monster, path[i]);
      if (appliedMove) {
        monster.currentMovementPoint--;
        monster.position = path[i];
      }
    }
  }

  tick() {
    if (!this.started) {
      return;
    }
    this.clock++;
    if (this.phase == 0) {
      if (this.clock > this.PREPARATION_TIME) {
        this.startFightPhase1();
      }
    } else {
      if (this.lockClock) return;
      if (this.clock > this.TURN_TIME) {
        this.nextTurn();
      }
    }
  }

  softResetPlayerStats(id: string) {
    const fighter = this.retrieveFighterFromPlayerId(id);
    fighter.currentActionPoint = fighter.actionPoint;
    fighter.currentMovementPoint = fighter.movementPoint;
  }

  nextTurn() {
    this.clock = 0;
    this.lockClock = false;
    this.softResetPlayerStats(this.acceptedId);
    let nextPlayer: Fighter;
    for (let i = 0; i < this.fightOrder.length; i++) {
      if (this.fightOrder[i].getId() == this.acceptedId) {
        if (i + 1 < this.fightOrder.length) {
          nextPlayer = this.fightOrder[i + 1];
        } else {
          nextPlayer = this.fightOrder[0];
        }
      }
    }

    this.acceptedId = nextPlayer.getId();
    this.io.to(this.id).emit("nextTurn", { playerId: nextPlayer.getId() });
    if (!nextPlayer.isRealPlayer) this.basicAI(<MonsterFighter>nextPlayer);
  }

  checkPlayerTurn(id: string): boolean {
    if (this.acceptedId == id) {
      return true;
    } else {
      return false;
    }
  }

  applyCell(fighter: Fighter, position: Position): boolean {
    return true;
  }

  useSpell(id: string, spell: ISpell, targetPosition: Position) {
    if (!this.checkPlayerTurn(id)) return;
    const fighter = this.retrieveFighterFromPlayerId(id);
    const processor = new SpellProcessor(
      this,
      spell,
      fighter,
      targetPosition,
      this.getFightersPosition().concat(this.map.getObstacles())
    );
    try {
      const impacts = processor.process();
      fighter.currentActionPoint -= spell.actionPointCost;
      this.applySpellImpacts(impacts);
      if (fighter.dead) this.nextTurn();
      this.updateTimeline();
      const fightIsfinished = this.isFightFinished();
      const humans = this.getTotalHumansFighter();
      for (const human of humans) {
        this.io.to(human.getSocketId()).emit("fighterUseSpell", {
          playerId: id,
          position: targetPosition,
          spellId: spell.id,
          impacts: impacts,
          checkin: fightIsfinished
            ? this.checkinManager.createSoftCheckin(human.getId(), this.sendFightResult)
            : undefined
        });
      }
    } catch (error) {
      console.log(error);
    }
  }

  monsterUseSpell(monster: MonsterFighter, spell: ISpell, targetPosition: Position): SpellImpact[] {
    const processor = new SpellProcessor(
      this,
      spell,
      monster,
      targetPosition,
      this.getFightersPosition().concat(this.map.getObstacles())
    );
    const impacts = processor.process();
    monster.currentActionPoint -= spell.actionPointCost;
    this.applySpellImpacts(impacts);
    this.updateTimeline();
    return impacts;
  }

  applySpellImpacts(impacts: SpellImpact[]) {
    for (let i = 0; i < impacts.length; i++) {
      const fighter = this.retrieveFighterFromPlayerId(impacts[i].playerId);
      fighter.currentLife += impacts[i].life;
      /* Need to apply other effect as Buff or Debuff */
      if (impacts[i].death) {
        fighter.dead = true;
      }
    }
  }

  updateTimeline() {
    for (let i = this.fightOrder.length - 1; i >= 0; --i) {
      if (this.fightOrder[i].dead) {
        this.fightOrder.splice(i, 1);
      }
    }
  }

  isFightFinished(): boolean {
    if (this.fightOrder.length == 1 || this.fightOrder.length == 0) return true;
    const baseSide: Side = this.fightOrder[0].side;
    for (const fighter of this.fightOrder) {
      if (fighter.side != baseSide) return false;
    }
    this.winningSide = baseSide;
    return true;
  }

  sendFightResult(socketId: string) {
    const fightEndProcessor = new FightEndProcessor();
    const human = this.retrieveHumanFighterFromSocketId(socketId);
    const fightResult = fightEndProcessor.process(human.side == this.winningSide);
    this.io.sockets.connected[human.getSocketId()].leave(this.id);
    human.player.leaveFight();
    this.io.to(human.getSocketId()).emit("fightResult", fightResult);
  }

  async basicAI(monster: MonsterFighter) {
    const processor = new AIProcessor(this, monster, this.map, this.blueTeam, this.redTeam, this.fightOrder);
    const impact = await processor.process();
    const commands: AICommand[] = [];
    let fightIsfinished: boolean;
    for (const action of impact.actions) {
      if (action.path) {
        const command = new AICommand();
        command.path = action.path;
        this.moveMonsterFighter(monster, action.path);
        commands.push(command);
      } else if (action.spell) {
        const command = new AICommand();
        command.spellId = action.spell.spell._id;
        command.targetPosition = action.spell.position;
        command.spellImpacts = this.monsterUseSpell(monster, action.spell.spell, command.targetPosition);
        fightIsfinished = this.isFightFinished();
        commands.push(command);
      }
    }

    const humans = this.getTotalHumansFighter();
    if (fightIsfinished) {
      for (const human of humans) {
        this.io.to(human.getSocketId()).emit("monsterCommand", {
          monsterId: monster.getId(),
          commands: commands,
          checkin: this.checkinManager.createSoftCheckin(human.getId(), this.sendFightResult)
        });
      }
    } else {
      const socketsIds = [];
      for (const human of humans) {
        socketsIds.push(human.getSocketId());
      }
      this.lockClock = true; // Clock is locked till everyone checked or the timeout run.
      const checkId = this.checkinManager.createComplexCheckin(
        socketsIds,
        this.everyOneReceivedIACommand,
        30,
        this.waitingForPlayer
      );
      for (const human of humans) {
        this.io.to(human.getSocketId()).emit("monsterCommand", {
          monsterId: monster.getId(),
          commands: commands,
          checkin: checkId
        });
      }
    }
  }

  everyOneReceivedIACommand(ids: string[]) {
    this.nextTurn();
  }

  waitingForPlayer(ids: { [id: string]: boolean }) {
    for (const id in ids) {
      if (ids[id] == false) {
        console.log("waiting for " + id + " checking. Timeouted"); // May send "En attente de ..." to room.
      }
    }

    this.nextTurn();
  }
}
