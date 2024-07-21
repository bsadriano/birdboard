import { useEffect, useState } from "react";
import { Project } from "../../../birdboard";
import { Link } from "react-router-dom";
import ProjectCard from "../../../Components/Projects/ProjectCard/ProjectCard";

interface Props {}

const ShowProject = (props: Props) => {
  const [project, setProject] = useState<Project>();

  useEffect(() => {
    setProject({
      id: 1,
      title: "title",
      description: "description",
      path: "/projects/1",
      tasks: [
        {
          id: 1,
          projectId: 1,
          body: "Task 1",
        },
      ],
      owner: {
        id: 1,
      },
    });
  }, []);

  return (
    <>
      <header className="flex items-center mb-3 py-4">
        <div className="flex justify-between items-end w-full">
          <p className="text-grey text-sm font-normal">
            <Link to="/projects">My Projects</Link> / {project?.title}
          </p>
          <Link className="button" to="/projects/create">
            New Project
          </Link>
        </div>
      </header>

      <main>
        <div className="lg:flex -mx-3 mb-6">
          <div className="lg:w-3/4 px-3">
            <div className="mb-8">
              <h2 className="text-grey font-normal text-lg mb-3">Tasks</h2>
              {project?.tasks &&
                project?.tasks?.map((project) => (
                  <div className="card mb-3">{project.body}</div>
                ))}
              <div className="card mb-3">
                <form action="#">
                  <input
                    type="text"
                    placeholder="Begin adding tasks..."
                    className="w-full"
                    name="body"
                  />
                </form>
              </div>
            </div>

            <div>
              <h2 className="text-grey font-normal text-lg mb-3">
                General Notes
              </h2>
              {/* General Notes */}
              <textarea className="card w-full" style={{ minHeight: "200px" }}>
                Lorem, ipsum.
              </textarea>
            </div>
          </div>

          <div className="lg:w-1/4 px-3">
            {project && <ProjectCard project={project} />}
          </div>
        </div>
      </main>
    </>
  );
};

export default ShowProject;
