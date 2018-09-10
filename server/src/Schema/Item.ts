import Mongoose from "mongoose";

export interface IItem extends Mongoose.Document {
  _id: Mongoose.Types.ObjectId;
  name: String;
}

export const ItemSchema = new Mongoose.Schema({
  name: String
});

const Item = Mongoose.model<IItem>("Object", ItemSchema);
export default Item;
