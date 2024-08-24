import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify";
import * as Yup from "yup";
import Agent from "../../../Api/Agent";
import SaveProjectForm from "../../../Components/Projects/SaveProjectForm/SaveProjectForm";
import { SaveProjectRequestDto } from "../../../Models/Project/ProjectRequestDto";

interface Props {}

const validation = Yup.object().shape({
  title: Yup.string()
    .required()
    .min(3, "Title cannot be less than 3 characters")
    .max(50, "Title cannot be over 50 characters"),
  description: Yup.string()
    .required()
    .min(3, "Description cannot be less than 3 characters")
    .max(50, "Description cannot be over 50 characters"),
});

const EditProject = (props: Props) => {
  const { projectId } = useParams();
  const navigate = useNavigate();
  const [project, setProject] = useState<SaveProjectRequestDto>();

  useEffect(() => {
    const getProject = async () => {
      const data = await Agent.Project.show(projectId!);

      if (data) {
        setProject({
          title: data.title,
          description: data.description,
        });
      }
    };

    if (projectId) {
      getProject();
    }
  }, [projectId]);

  const handleUpdateProject = async (body: SaveProjectRequestDto) => {
    try {
      const data = await Agent.Project.update(projectId!, body);
      if (data) {
        toast.success("Project updated!", {
          autoClose: 1000,
        });
        navigate(`/projects/${data.id}`);
      }
    } catch (error: any) {
      toast.warning(error);
    }
  };

  return (
    <>
      <div className="lg:w-1/2 lg:mx-auto bg-card p-6 md:py-12 md:px-16 rounded shadow">
        <h1 className="text-2xl font-normal mb-10 text-center text-default">
          Update Your Project
        </h1>
        {project && (
          <SaveProjectForm
            validation={validation}
            defaultValues={project}
            onSubmit={handleUpdateProject}
            projectId={projectId}
            isEdit={true}
          />
        )}
      </div>
    </>
  );
};

export default EditProject;
