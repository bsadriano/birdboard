import { Link } from "react-router-dom";
import { stringLimit } from "../../../Helpers/StringLimit";
import { ProjectGet } from "../../../Models/Project";

interface Props {
  project: ProjectGet;
}

const ProjectCard = ({ project }: Props) => {
  return (
    <div className="card" style={{ height: "200px" }}>
      <h3 className="font-normal text-xl py-4 -ml-5 mb-3 border-l-4 border-blue-light pl-4">
        <Link to={`/projects/${project.id}`}>{project.title}</Link>
      </h3>
      <div className="text-grey">{stringLimit(project.description, 100)}</div>
    </div>
  );
};

export default ProjectCard;
