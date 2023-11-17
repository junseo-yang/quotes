import { useEffect, useState } from "react";
import Tag from "./components/Tag"

function App() {
  const [data, setData] = useState([]);
  const [tag, setTag] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [text, setText] = useState("")
  const [isUpdating, setIsUpdating] = useState(false)
  const [tagId, setTagId] = useState("")

  useEffect(() => {
    getTags();
  }, []);

  const getTags = async () => {
    try {
      const response = await fetch('https://localhost:7223/api/tags', {
        method: "GET",
        mode: "cors",
        cache: "no-cache",
        headers: {
          "Content-Type": "application/json"
        }
      });
      if (!response.ok) {
        throw new Error('Network response was not ok.');
      }
      const data = await response.json();
      setData(data);
      setTag(data.tags);
      setLoading(false);
    } catch (error) {
      setError(error.message);
      setLoading(false);
    }
  };

  const addTag = async (text, setText, setTag) => {
    try {
      const response = await fetch('https://localhost:7223/api/tags', {
        method: "POST",
        mode: "cors",
        cache: "no-cache",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(
          {
            "name": text 
          }
        )
      });
      if (!response.ok) {
        throw new Error('Network response was not ok.');
      }
      const data = await response.json();
      setText("")
      getTags(setTag)
    } catch (error) {
      console.error(error);
      setError(error.message);
      setLoading(false);
    }
  }

  const updateMode = (tagId, name) => {
    setIsUpdating(true)
    setText(name)
    setTagId(tagId)
  }

  const updateTag = async (tagId, text, setText, setTag, setIsUpdating) => {
    try {
      const response = await fetch('https://localhost:7223/api/tags', {
        method: "PUT",
        mode: "cors",
        cache: "no-cache",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify(
          {
            "tagId": tagId,
            "name": text
          }
        )
      });
      if (!response.ok) {
        throw new Error('Network response was not ok.');
      }
      const data = await response.json();
      setText("")
      setIsUpdating(false)
      getTags(setTag)
    } catch (error) {
      console.error(error);
      setError(error.message);
    }
  }

  return (
    <div className="App">
      <div className="container">
        <h1>Tags</h1>
        <div className="top">
          <input 
            type="text"
            placeholder="Add Tags"
            value={text}
            onChange={(e) => setText(e.target.value)}
          />
          <div
            className="add"
            onClick={
              isUpdating ? 
                () => updateTag(tagId, text, setText, setTag, setIsUpdating)
                : () => addTag(text, setText, setTag)}
          >
            {isUpdating ? "Update" : "Add"}
          </div>
        </div>

        <div className="list">
          {tag.map((item) => 
            <Tag 
              key={item.tagId} 
              text={item.name}
              updateMode={() => updateMode(item.tagId, item.name)}
            />
          )}
        </div>
      </div>
    </div>
  );
}

export default App;
