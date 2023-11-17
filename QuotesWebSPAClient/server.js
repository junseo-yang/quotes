const express = require("express");
const path = require("path");

const app = express();

app.use("/static", express.static(path.resolve(__dirname, "frontend", "static")));

app.use("/lib", express.static(path.resolve(__dirname, "frontend", "lib")));

app.get("/*", (req, res) => {
    res.sendFile(path.resolve(__dirname, "frontend", "index.html"));
});

app.listen(process.env.PORT || 3000, () => {
    console.log("Server running on 3000...");
    console.log("Ctrl + Click to open http://localhost:3000/");
    console.log("Ctrl + C to stop the server.");
});
