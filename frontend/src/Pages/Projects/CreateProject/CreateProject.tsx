import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { Link, useNavigate } from "react-router-dom";
import * as Yup from "yup";
import {
  AddProjectData,
  projectsPostAPI,
} from "../../../Services/ProjectService";
import { toast } from "react-toastify";

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

const CreateProject = (props: Props) => {
  const navigate = useNavigate();
  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<AddProjectData>({ resolver: yupResolver(validation) });

  const handleAddProject = (projectData: AddProjectData) => {
    projectsPostAPI(projectData)
      .then((res) => {
        if (res) {
          toast.success("Project created!");
          navigate(`/projects/${res.data.id}`);
        }
      })
      .catch((e) => {
        toast.warning(e);
      });
    //
  };

  return (
    <form onSubmit={handleSubmit(handleAddProject)}>
      <h1>Create a Project</h1>

      <div>
        <label htmlFor="title">Title</label>
        <div>
          <input type="text" placeholder="Title" {...register("title")} />
          {errors.title && (
            <p className="text-red-400 text-sm">{errors.title.message}</p>
          )}
        </div>
      </div>

      <div>
        <label htmlFor="description">Description</label>
        <div>
          <textarea {...register("description")} />
          {errors.description && (
            <p className="text-red-400 text-sm">{errors.description.message}</p>
          )}
        </div>
      </div>

      <div>
        <button type="submit">Create Project</button>
        <Link to="/projects">Cancel</Link>
      </div>
    </form>
  );
};

export default CreateProject;
