import Mongoose from "mongoose";

export interface IScenario extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  requirement: {
    minLevel: number;
    maxLevel: number;
    activeQuests: Mongoose.Types.ObjectId[];
    completedQuests: Mongoose.Types.ObjectId[];
  };
  discussions: string[];
  isQuest: boolean;
}

export const ScenarioSchema = new Mongoose.Schema({
  requirements: {
    minLevel: { type: Number, default: 0 },
    maxLevel: { type: Number, default: 120 },
    activeQuests: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Quest" }],
    completedQuests: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Quest" }]
  },
  discussions: [String],
  isQuest: Boolean
});

const Scenario = Mongoose.model<IScenario>("Stats", ScenarioSchema);
export default Scenario;
