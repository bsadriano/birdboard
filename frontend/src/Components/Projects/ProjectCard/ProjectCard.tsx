import { Link } from "react-router-dom";
import { stringLimit } from "../../../Helpers/StringLimit";
import { ProjectGet } from "../../../Models/Project";
import { FormEvent } from "react";
import { projectsDeleteAPI } from "../../../Services/ProjectService";
import { toast } from "react-toastify";

interface Props {
  project: ProjectGet;
  onDelete?: () => void;
}

const ProjectCard = ({ project, onDelete = () => {} }: Props) => {
  function handleDelete(event: FormEvent<HTMLFormElement>): void {
    event.preventDefault();

    projectsDeleteAPI(project.id)
      .then((res) => {
        if (res) {
          toast.success("Project deleted!", {
            autoClose: 1000,
          });

          onDelete();
        }
      })
      .catch((e) => {
        toast.warning(e);
      });
  }

  return (
    <div className="card" style={{ height: "200px" }}>
      <h3 className="font-normal text-xl py-4 -ml-5 mb-3 border-l-4 border-blue-light pl-4">
        <Link to={`/projects/${project.id}`}>{project.title}</Link>
      </h3>
      <div className="text-grey mb-4">
        {stringLimit(project.description, 100)}
      </div>

      <footer>
        <form onSubmit={handleDelete}>
          <div className="text-right">
            <button type="submit" className="text-xs">
              Delete
            </button>
          </div>
        </form>
      </footer>
    </div>
  );
};

export default ProjectCard;
