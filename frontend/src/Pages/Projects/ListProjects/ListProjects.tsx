import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import Agent from "../../../Api/Agent";
import ProjectCard from "../../../Components/Projects/ProjectCard/ProjectCard";
import SaveProjectModal from "../../../Components/Projects/SaveProjectModal/SaveProjectModal";
import { ProjectResponseDto } from "../../../Models/Project/ProjectResponseDto";

interface Props {}

const ListProjects = (props: Props) => {
  const [projects, setProjects] = useState<ProjectResponseDto[]>([]);
  useEffect(() => {
    getProjects();
  }, []);

  const getProjects = async () => {
    try {
      const data = await Agent.Project.list();

      if (data) {
        setProjects(data);
      }
    } catch (error) {
      console.log(error);
      toast.warning("Could not get projects!");
    }
  };

  const [isOpen, setIsOpen] = useState(false);

  function openModal() {
    setIsOpen(true);
  }

  function closeModal() {
    setIsOpen(false);
  }

  return (
    <>
      <header className="flex items-center mb-3 py-4">
        <div className="flex justify-between items-end w-full">
          <h2 className="text-default text-sm font-normal">My Projects</h2>
          <button className="button" onClick={openModal}>
            New Project
          </button>
        </div>
      </header>
      <main className="lg:flex lg:flex-wrap -mx-3">
        {projects ? (
          projects.map((project) => (
            <div key={project.id} className="lg:w-1/3 px-3 pb-6">
              <ProjectCard project={project} onDelete={getProjects} />
            </div>
          ))
        ) : (
          <div>No projects yet.</div>
        )}
      </main>

      <SaveProjectModal isOpen={isOpen} closeModal={closeModal} />
    </>
  );
};

export default ListProjects;
