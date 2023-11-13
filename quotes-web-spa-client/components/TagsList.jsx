import Link from "next/link";
import RemoveBtn from "./RemoveBtn";
import { HiPencilAlt } from "react-icons/hi";

const getTags = async () => {
  const resp = await fetch("https://localhost:7223/api/tags", {
      mode: "cors",
      headers: {
          'Accept': 'application/json'
      }
  })
  // The return value is *not* serialized
  // You can return Date, Map, Set, etc.
 
  if (!res.ok) {
    // This will activate the closest `error.js` Error Boundary
    throw new Error('Failed to fetch data')
  }
 
  return res.json()
  // try {
  //   const res = await fetch("", {
  //     cache: "no-store",
  //   });

  //   if (!res.ok) {
  //     throw new Error("Failed to fetch data");
  //   }

  //   return res.json();
  // } catch (error) {
  //   console.log("Error loading tags: ", error);
  // }
};

export default async function TagsList() {
  const { tags } = await getTags();

  return (
    <>
      {/* {tags.map((t) => (
      ))} */}
      <div>
        <div className="flex justify-end m-3">
          <Link className="font-bold text-1xl" href={"/addTag"}>
            Add Tag
          </Link>
        </div>

        <div className="p-4 border border-slate-300 my-3 flex justify-between gap-5 items-start">
          <div>
            <h2 className="font-bold text-2xl">Name</h2>
            <div>Tag Title</div>
          </div>

          <div className="flex gap-2">
            <RemoveBtn />
            <Link href={"/editTag/123"}>
              <HiPencilAlt size={24} />
            </Link>
          </div>
        </div>
      </div>
    </>
  );
}
