import React, { useEffect, useState } from "react";
import { Project } from "../../../birdboard";
import { Link } from "react-router-dom";

interface Props {}

const ShowProject = (props: Props) => {
  const [project, setProject] = useState<Project>();

  useEffect(() => {
    setProject({
      title: "title",
      description: "description",
      path: "/projects/1",
    });
  }, []);

  return (
    <>
      <h1>{project?.title}</h1>
      <div>{project?.description}</div>
      <Link to="/projects">Go Back</Link>
    </>
  );
};

export default ShowProject;
