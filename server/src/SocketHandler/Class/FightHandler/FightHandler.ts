import GSocket from "../../../BusinessClasses/GSocket";
import MapController from "../../../BusinessClasses/MapController";
import PlayerController from "../../../BusinessClasses/PlayerController";
import State from "../../../BusinessClasses/State";
import FightController from "../../../BusinessClasses/FightController";
import Position from "../../../BusinessClasses/RelationalObject/Position";
import Spell from "../../../Schema/Spell";
import MonsterGroup, { IMonsterGroup } from "../../../Schema/MonsterGroup";
import MonsterController from "../../../BusinessClasses/MonsterController";

export default class FightHandler {
  socket: GSocket;
  M: MapController;
  P: PlayerController;
  F: FightController;
  E: MonsterController;

  constructor(socket: GSocket, state: State) {
    this.socket = socket;
    this.M = state.MapController;
    this.P = state.PlayerController;
    this.F = state.FightController;
    this.E = state.MonsterController;
    this.initSocket();
  }

  initSocket() {
    this.socket.on("startFight", this.startFight.bind(this));
    this.socket.on("startMonsterFight", this.startMonsterFight.bind(this));
    this.socket.on("teleportPreFight", this.teleportPreFight.bind(this));
    this.socket.on("fighterReady", this.fighterReady.bind(this));
    this.socket.on("fighterFinishTurn", this.fighterFinishTurn.bind(this));
    this.socket.on("fighterMove", this.fighterMove.bind(this));
    this.socket.on("fighterUseSpell", this.fighterUseSpell.bind(this));
  }

  async startFight(target: { id: string }) {
    await this.socket.player.enterInFight();
    const firstTeam = [this.socket.player];
    const secondTeam = await this.P.RetrievePlayers([target.id]);
    for (let i = 0; i < secondTeam.length; i++) {
      await secondTeam[i].enterInFight();
    }
    const map = await this.M.getMap(this.socket.player.mapPosition.x, this.socket.player.mapPosition.y);
    this.F.startFight(firstTeam, secondTeam, map);
  }

  async startMonsterFight(target: { id: string }) {
    const secondTeam: IMonsterGroup = await MonsterGroup.findById(target.id);
    if (!secondTeam) return;
    const map = await this.M.getMap(this.socket.player.mapPosition.x, this.socket.player.mapPosition.y);
    let monsterGroupFound = false;
    let groupIndex = -1;
    for (let i = 0; i < map.monsterGroups.length; i++) {
      if (map.monsterGroups[i].equals(target.id)) {
        monsterGroupFound = true;
        groupIndex = i;
      }
    }
    if (!monsterGroupFound) return;
    map.monsterGroups.splice(groupIndex, 1);
    await map.save();
    await this.socket.player.enterInFight();
    const firstTeam = [this.socket.player];


    // this.F.startFight(firstTeam, secondTeam, map);
  }

  async teleportPreFight(data: { x: number; y: number; fightId: string }) {
    const fight = this.F.retrieveFight(data.fightId);
    if (!fight) return;
    try {
      const fighter = fight.retrieveFighterFromPlayerId(this.socket.player.id);
      if (!fighter.ready) fight.teleportPlayerPhase0(fighter, new Position(data.x, data.y));
    } catch (error) {
      console.log(error);
    }
  }

  async fighterReady(data: { fightId: string }) {
    const fight = this.F.retrieveFight(data.fightId);
    if (!fight) return;
    try {
      fight.setFighterReady(this.socket.player.id);
    } catch (error) {
      console.log(error);
    }
  }
  async fighterFinishTurn(data: { fightId: string }) {
    const fight = this.F.retrieveFight(data.fightId);
    if (!fight) return;
    try {
      if (fight.checkPlayerTurn(this.socket.player.id)) {
        fight.nextTurn();
      }
    } catch (error) {
      console.log(error);
    }
  }

  async fighterMove(data: { fightId: string; path: Position[] }) {
    const fight = this.F.retrieveFight(data.fightId);
    if (!fight) return;
    if (data.path.length == 0) return;
    const possible = await this.M.checkMovementsPossibility(this.socket, data.path);
    if (possible) {
      try {
        fight.moveFighter(this.socket.player.id, data.path);
      } catch (error) {
        console.log(error);
      }
    }
  }

  async fighterUseSpell(data: { fightId: string; spellId: string; position: Position }) {
    const fight = this.F.retrieveFight(data.fightId);
    if (!fight) return;
    const possible = await this.M.checkMovementPossibility(this.socket, data.position);
    try {
      if (possible && this.socket.player.hasSpell(data.spellId)) {
        const spell = await Spell.findById(data.spellId);
        await fight.useSpell(this.socket.player.id, spell, data.position);
        if (fight.isFinished) {
          this.F.removeFight(data.fightId);
        }
      }
    } catch (error) {
      console.log(error);
    }
  }
}
