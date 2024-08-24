import { FormEvent } from "react";
import { Link } from "react-router-dom";
import { toast } from "react-toastify";
import Agent from "../../../Api/Agent";
import { useAuth } from "../../../Context/useAuth";
import { stringLimit } from "../../../Helpers/StringLimit";
import { ProjectResponseDto } from "../../../Models/Project/ProjectResponseDto";

interface Props {
  project: ProjectResponseDto;
  onDelete?: () => void;
}

const ProjectCard = ({ project, onDelete = () => {} }: Props) => {
  const { isAuthUser } = useAuth();

  async function handleDelete(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    try {
      await Agent.Project.delete(project.id);
      toast.success("Project deleted!", {
        autoClose: 1000,
      });

      onDelete();
    } catch (e: any) {
      toast.warning(e);
    }
  }

  return (
    <div className="card flex flex-col" style={{ height: "200px" }}>
      <h3 className="font-normal text-xl py-4 -ml-5 mb-3 border-l-4 border-accent pl-4">
        <Link to={`/projects/${project.id}`}>{project.title}</Link>
      </h3>

      <div className="mb-4 flex-1">{stringLimit(project.description, 100)}</div>

      {isAuthUser(project.owner.email) && (
        <footer>
          <form onSubmit={handleDelete}>
            <div className="text-right">
              <button type="submit" className="text-xs">
                Delete
              </button>
            </div>
          </form>
        </footer>
      )}
    </div>
  );
};

export default ProjectCard;
