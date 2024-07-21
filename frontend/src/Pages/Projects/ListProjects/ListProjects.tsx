import React, { useEffect, useState } from "react";
import { Project } from "../../../birdboard";
import { Link } from "react-router-dom";

interface Props {}

const ListProjects = (props: Props) => {
  const [projects, setProjects] = useState<Project[]>([]);
  useEffect(() => {
    setProjects([
      { title: "title", description: "description", path: "/projects/1" },
      { title: "title", description: "description", path: "/projects/2" },
      { title: "title", description: "description", path: "/  projects/3" },
    ]);
  }, []);
  return (
    <>
      <div className="flex items-center mb-3">
        <Link to="/projects/create">New Project</Link>
      </div>

      <ul>
        {projects ? (
          projects.map((project) => (
            <li>
              <Link to={project.path}>{project.title}</Link>
            </li>
          ))
        ) : (
          <li>No projects yet.</li>
        )}
      </ul>
    </>
  );
};

export default ListProjects;
