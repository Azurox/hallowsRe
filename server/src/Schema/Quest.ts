import Mongoose from "mongoose";

export interface IQuest extends Mongoose.Document {}

export const QuestSchema = new Mongoose.Schema({});

const Quest = Mongoose.model<IQuest>("Stats", QuestSchema);
export default Quest;
