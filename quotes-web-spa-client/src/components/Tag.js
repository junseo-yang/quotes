import React from "react";
import {BiEdit} from "react-icons/bi"

const Tag = ({text, updateMode, deleteTag}) => {
  return (
    <div className="tag">
        <div className="text">{text}</div>
        <div className="icons">
            <BiEdit className="icon" onClick={updateMode} />
        </div>
    </div>
  );
};

export default Tag;
