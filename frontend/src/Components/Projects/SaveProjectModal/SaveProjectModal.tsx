import { yupResolver } from "@hookform/resolvers/yup";
import { camelCase } from "change-case";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import * as Yup from "yup";
import Agent from "../../../Api/Agent";
import { SaveProjectRequestDto } from "../../../Models/Project/ProjectRequestDto";
import Modal from "../../Modal/Modal";

interface Props {
  isOpen: boolean;
  closeModal: () => void;
}

const validationSchema = Yup.object().shape({
  title: Yup.string()
    .required("Title is required")
    .min(3, "Title cannot be less than 3 characters")
    .max(50, "Title cannot be over 50 characters"),
  description: Yup.string()
    .required("Description is required")
    .min(3, "Description cannot be less than 3 characters")
    .max(50, "Description cannot be over 50 characters"),
  tasks: Yup.array().of(
    Yup.object().shape({
      body: Yup.string()
        .test(
          "empty-or-gte-3-characters-check",
          "Task cannot be less than 3 characters",
          (task) => !task || task.length >= 3
        )
        .test(
          "empty-or-lte-50-characters-check",
          "Task cannot be over 50 characters",
          (task) => !task || task.length <= 50
        ),
    })
  ),
});

const SaveProjectModal = ({ isOpen, closeModal }: Props) => {
  const navigate = useNavigate();
  const {
    control,
    handleSubmit,
    register,
    setError,
    formState: { errors },
  } = useForm<SaveProjectRequestDto>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      title: "",
      description: "",
      tasks: [{ body: "" }],
    },
    mode: "onSubmit",
    shouldUnregister: false,
  });

  const { fields, append } = useFieldArray({
    control,
    name: "tasks",
  });

  const addTask = () => {
    append({ body: "" });
  };

  const handleAddProject = async (body: SaveProjectRequestDto) => {
    try {
      const data = await Agent.Project.create(body);
      if (data) {
        closeModal();
        toast.success("Project created!", {
          autoClose: 1000,
        });
        navigate(`/projects/${data.id}`);
      }
    } catch (e: any) {
      type FormField =
        | "title"
        | "description"
        | "tasks"
        | `tasks.${number}`
        | `tasks.${number}.body`;

      interface FormErrors {
        [key: string]: string[];
      }

      const errors: FormErrors = e?.data?.errors;

      if (errors) {
        Object.keys(errors).forEach((key) => {
          setError(camelCase(key) as FormField, {
            type: "manual",
            message: errors[key][0],
          });
        });
      }

      console.log(e.data.errors);
      toast.warning(e);
    }
  };

  return (
    <Modal isOpen={isOpen}>
      <h1 className="font-normal mb-16 text-2xl text-center text-default">
        Let's Start Something New
      </h1>
      <form onSubmit={handleSubmit(handleAddProject)}>
        <div className="flex">
          <div className="flex-1 mr-4">
            <div className="mb-4">
              <label
                htmlFor="title"
                className="text-sm text-default block mb-2"
              >
                Title
              </label>
              <input
                type="text"
                id="title"
                className={`w-full block bg-card rounded border p-2 text-xs text-default ${
                  errors.title ? "border-error" : "border-muted-light"
                }`}
                {...register("title")}
              />

              {errors.title && (
                <p className="text-error text-xs">{errors.title.message}</p>
              )}
            </div>
            <div className="mb-4">
              <label
                htmlFor="description"
                className="block mb-2 text-sm text-default"
              >
                Description
              </label>
              <textarea
                id="description"
                className={
                  `w-full block bg-card rounded border p-2 text-xs text-default ` +
                  (errors.description ? "border-error" : "border-muted-light")
                }
                rows={7}
                {...register("description")}
              ></textarea>

              {errors.description && (
                <p className="text-error text-xs">
                  {errors.description.message}
                </p>
              )}
            </div>
          </div>

          <div className="flex-1 ml-4">
            <div className="mb-4">
              <label className="block mb-2 text-sm text-default">
                Need Some Tasks?
              </label>
              {fields.map((field, index) => (
                <div key={field.id} className="mb-2">
                  <Controller
                    name={`tasks.${index}.body`}
                    control={control}
                    defaultValue={field.body || ""}
                    render={({ field }) => (
                      <input
                        type="text"
                        className={
                          `w-full block bg-card rounded border p-2 text-xs text-default ` +
                          (errors.tasks?.[index]?.body
                            ? "border-error"
                            : "border-muted-light")
                        }
                        placeholder="Task"
                        {...field}
                      />
                    )}
                  />
                  {errors.tasks?.[index]?.body && (
                    <p className="text-error text-xs">
                      {errors.tasks[index]?.body?.message}
                    </p>
                  )}
                </div>
              ))}
            </div>

            <button
              type="button"
              className="inline-flex items-center text-xs text-default"
              onClick={addTask}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="18"
                height="18"
                viewBox="0 0 18 18"
                className="mr-2"
              >
                <g
                  fill="none"
                  fillRule="evenodd"
                  opacity=".307"
                  className="stroke-current"
                >
                  <path
                    stroke="#000"
                    strokeOpacity=".012"
                    strokeWidth="0"
                    d="M-3-3h24v24H-3z"
                  ></path>
                  <path
                    fill="#000"
                    d="M9 0a9 9 0 0 0-9 9c0 4.97 4.02 9 9 9A9 9 0 0 0 9 0zm0 16c-3.87 0-7-3.13-7-7s3.13-7 7-7 7 3.13 7 7-3.13 7-7 7zm1-11H8v3H5v2h3v3h2v-3h3V8h-3V5z"
                  ></path>
                </g>
              </svg>
              <span>Add New Task Field</span>
            </button>
          </div>
        </div>
        <footer className="flex justify-end">
          <button
            type="button"
            className="button is-outlined mr-4"
            onClick={closeModal}
          >
            Cancel
          </button>
          <button type="submit" className="button">
            Create Project
          </button>
        </footer>
      </form>
    </Modal>
  );
};

export default SaveProjectModal;
