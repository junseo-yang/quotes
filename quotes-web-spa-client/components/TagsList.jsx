import Link from "next/link";
import RemoveBtn from "./RemoveBtn";
import { HiPencilAlt } from "react-icons/hi";

export default function TagsList() {
  return (
    <>
      <div className="flex justify-end m-3">
        <Link className="font-bold text-1xl" href={"/addTag"}>
          Add Tag
        </Link>
      </div>

      <div className="p-4 border border-slate-300 my-3 flex justify-between gap-5 items-start">
        <div>
          <h2 className="font-bold text-2xl">Tag Id</h2>
          <div>Tag Name</div>
        </div>

        <div className="flex gap-2">
          <RemoveBtn />
          <Link href={"/editTag/123"}>
            <HiPencilAlt size={24} />
          </Link>
        </div>
      </div>
    </>
  );
}
