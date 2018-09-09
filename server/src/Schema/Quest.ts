import Mongoose from "mongoose";

export interface IQuest extends Mongoose.Document {}

export const QuestSchema = new Mongoose.Schema({});

const Quest = Mongoose.model<IQuest>("Quest", QuestSchema);
export default Quest;
