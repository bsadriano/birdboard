import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { toast } from "react-toastify";
import * as Yup from "yup";
import SaveProjectForm from "../../../Components/Projects/SaveProjectForm/SaveProjectForm";
import {
  projectGetAPI,
  projectPatchAPI,
} from "../../../Services/ProjectService";

interface Props {}

export type EditProjectFormInputs = {
  title: string;
  description: string;
};

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
  const [project, setProject] = useState<EditProjectFormInputs>();

  useEffect(() => {
    if (projectId) {
      getProject();
    }
  }, [projectId]);

  const getProject = async () => {
    const res = await projectGetAPI(projectId!);

    if (res) {
      setProject({
        title: res.data.title,
        description: res.data.description,
      });
    }

    return null;
  };

  const handleUpdateProject = async (projectData: EditProjectFormInputs) => {
    projectPatchAPI(projectId!, projectData)
      .then((res) => {
        if (res) {
          toast.success("Project updated!", {
            autoClose: 1000,
          });
          navigate(`/projects/${res.data.id}`);
        }
      })
      .catch((e) => {
        toast.warning(e);
      });
  };

  return (
    <>
      <div className="lg:w-1/2 lg:mx-auto bg-white p-6 md:py-12 md:px-16 rounded shadow">
        <h1 className="text-2xl font-normal mb-10 text-center">
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
