export default class Position {
  x: number;
  y: number;

  constructor(x: number, y: number) {
    this.x = x;
    this.y = y;
  }

  equals(position: Position): boolean {
    if (this.x == position.x && this.y == position.y) return true;
    return false;
  }

  toSimplePosition(): { x: number; y: number } {
    return {
      x: this.x,
      y: this.y
    };
  }

  static ToPositions(simplePosition: { x: number; y: number }[]): Position[] {
    const positions = [];
    for (let i = 0; i < simplePosition.length; i++) {
      positions.push(new Position(simplePosition[i].x, simplePosition[i].y));
    }
    return positions;
  }

  static ToPosition(simplePosition: { x: number; y: number }): Position {
    return new Position(simplePosition.x, simplePosition.y);
  }
}
