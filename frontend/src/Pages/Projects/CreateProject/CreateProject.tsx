import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import * as Yup from "yup";
import Agent from "../../../Api/Agent";
import SaveProjectForm from "../../../Components/Projects/SaveProjectForm/SaveProjectForm";
import { SaveProjectRequestDto } from "../../../Models/Project/ProjectRequestDto";

interface Props {}

const validation = Yup.object<SaveProjectRequestDto>().shape({
  title: Yup.string()
    .required("The title field is required")
    .min(3, "Title cannot be less than 3 characters")
    .max(50, "Title cannot be over 50 characters"),
  description: Yup.string()
    .required("The description field is required")
    .min(3, "Description cannot be less than 3 characters")
    .max(50, "Description cannot be over 50 characters"),
});

const CreateProject = (props: Props) => {
  const navigate = useNavigate();

  const handleAddProject = async (body: SaveProjectRequestDto) => {
    try {
      const data = await Agent.Project.create(body);
      if (data) {
        toast.success("Project created!", {
          autoClose: 1000,
        });
        navigate(`/projects/${data.id}`);
      }
    } catch (error: any) {
      toast.warning(error);
    }
  };

  return (
    <div className="lg:w-1/2 lg:mx-auto bg-white p-6 md:py-12 md:px-16 rounded shadow">
      <h1 className="text-2xl font-normal mb-10 text-center">
        Let's start something new
      </h1>

      <SaveProjectForm
        defaultValues={{ title: "", description: "" }}
        validation={validation}
        onSubmit={handleAddProject}
      />
    </div>
  );
};

export default CreateProject;
