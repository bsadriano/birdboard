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
    <form
      className="lg:w-1/2 lg:mx-auto bg-white p-6 md:py-12 md:px-16 rounded shadow"
      onSubmit={handleSubmit(handleAddProject)}
    >
      <h1 className="text-2xl font-normal mb-10 text-center">
        Create a Project
      </h1>

      <div className="mb-6">
        <label className="text-sm mb-2 block" htmlFor="title">
          Title
        </label>
        <div>
          <input
            type="text"
            className="bg-transparent border border-grey-light rounded p-2 text-xs w-full"
            placeholder="My next awesome project"
            {...register("title")}
          />
          {errors.title && (
            <p className="text-red-400 text-sm">{errors.title.message}</p>
          )}
        </div>
      </div>

      <div className="mb-6">
        <label className="text-sm mb-2 block" htmlFor="description">
          Description
        </label>
        <div>
          <textarea
            className="bg-transparent border border-grey-light rounded p-2 text-xs w-full"
            rows={10}
            placeholder="I should start learning piano"
            {...register("description")}
          />
          {errors.description && (
            <p className="text-red-400 text-sm">{errors.description.message}</p>
          )}
        </div>
      </div>

      <div>
        <button className="button mr-4" type="submit">
          Create Project
        </button>
        <Link to="/projects">Cancel</Link>
      </div>
    </form>
  );
};

export default CreateProject;
