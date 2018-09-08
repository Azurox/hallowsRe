import Mongoose from "mongoose";
import Position from "../BusinessClasses/RelationalObject/Position";

export interface INpc extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: string;
  scenarios: Mongoose.Types.ObjectId[];
  mapPosition: Position;
  position: Position;
  orientation: number;
  imageId: string;
}

export const NpcSchema = new Mongoose.Schema({
  name: String,
  scenarios: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Scenario" }],
  mapPosition: { x: Number, y: Number },
  position: { x: Number, y: Number },
  orientation: Number,
  imageId: String
});

const Npc = Mongoose.model<INpc>("Npc", NpcSchema);
export default Npc;
