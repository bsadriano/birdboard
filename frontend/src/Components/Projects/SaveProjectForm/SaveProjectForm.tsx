import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import { ProjectFormInputs } from "../../../Services/ProjectService";

interface Props {
  defaultValues: any;
  validation: any;
  onSubmit: any;
  projectId?: string;
  isEdit?: boolean;
}

const SaveProjectForm = ({
  defaultValues,
  validation,
  onSubmit,
  isEdit = false,
  projectId,
}: Props) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<ProjectFormInputs>({
    defaultValues: defaultValues,
    resolver: yupResolver(validation),
  });
  return (
    <form onSubmit={handleSubmit(onSubmit)}>
      <div className="mb-6">
        <label className="text-sm mb-2 block" htmlFor="title">
          Title
        </label>
        <div>
          <input
            type="text"
            className="bg-transparent border border-grey-light rounded p-2 text-xs w-full"
            placeholder="My next awesome project"
            required
            {...register("Title")}
          />
          {errors.Title && (
            <p className="text-red-400 text-sm">{errors.Title.message}</p>
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
            required
            {...register("Description")}
          />
          {errors.Description && (
            <p className="text-red-400 text-sm">{errors.Description.message}</p>
          )}
        </div>
      </div>

      <div>
        <button className="button mr-4" type="submit">
          {isEdit ? "Update Project" : "Create Project"}
        </button>
        <Link to={isEdit ? `/projects/${projectId}` : "/projects"}>Cancel</Link>
      </div>
    </form>
  );
};

export default SaveProjectForm;
