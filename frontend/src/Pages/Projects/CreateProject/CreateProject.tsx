import React from "react";
import { Link } from "react-router-dom";

interface Props {}

const CreateProject = (props: Props) => {
  return (
    <>
      <form action="/projects" method="POST">
        <h1>Create a Project</h1>

        <div>
          <label htmlFor="title">Title</label>
          <div>
            <input type="text" name="title" placeholder="Title" />
          </div>
        </div>

        <div>
          <label htmlFor="description">Description</label>
          <div>
            <textarea name="description" />
          </div>
        </div>

        <div>
          <button type="submit">Create Project</button>
          <Link to="/projects">Cancel</Link>
        </div>
      </form>
    </>
  );
};

export default CreateProject;
