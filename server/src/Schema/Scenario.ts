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
  responses: {
    completeQuest: Mongoose.Types.ObjectId;
    startQuest: Mongoose.Types.ObjectId;
  }[];
}

export const ScenarioSchema = new Mongoose.Schema({
  requirements: {
    minLevel: { type: Number, default: 0 },
    maxLevel: { type: Number, default: 120 },
    activeQuests: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Quest" }],
    completedQuests: [{ type: Mongoose.Schema.Types.ObjectId, ref: "Quest" }]
  },
  discussions: [String],
  isQuest: Boolean,
  responses: [
    {
      completeQuest: { type: Mongoose.Schema.Types.ObjectId, ref: "Quest" },
      startQuest: { type: Mongoose.Schema.Types.ObjectId, ref: "Quest" }
    }
  ]
});

const Scenario = Mongoose.model<IScenario>("Quest", ScenarioSchema);
export default Scenario;
