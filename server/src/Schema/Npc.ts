import Mongoose from "mongoose";

export interface INpc extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: string;
  scenarios: Mongoose.Types.ObjectId[];
  position: Position;
  orientation: number;
  imageId: string;
}

export const NpcSchema = new Mongoose.Schema({
  name: String,
  scenarios: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Scenario" }],
  position: { x: Number, y: Number },
  orientation: Number,
  imageId: String
});

const Npc = Mongoose.model<INpc>("Stats", NpcSchema);
export default Npc;
