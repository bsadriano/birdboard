import { yupResolver } from "@hookform/resolvers/yup";
import { Controller, useFieldArray, useForm } from "react-hook-form";
import * as Yup from "yup";
import Modal from "../../Modal/Modal";
import { projectsPostAPI } from "../../../Services/ProjectService";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";

interface Props {
  isOpen: boolean;
  closeModal: () => void;
}

type Task = {
  Body?: string;
};

type ProjectFormInputs = {
  Title: string;
  Description: string;
  Tasks?: Task[];
};

const validationSchema = Yup.object().shape({
  Title: Yup.string()
    .required("Title is required")
    .min(3, "Title cannot be less than 3 characters")
    .max(50, "Title cannot be over 50 characters"),
  Description: Yup.string()
    .required("Description is required")
    .min(3, "Description cannot be less than 3 characters")
    .max(50, "Description cannot be over 50 characters"),
  Tasks: Yup.array().of(
    Yup.object().shape({
      Body: Yup.string()
        .min(3, "Task cannot be less than 3 characters")
        .max(50, "Task cannot be over 50 characters"),
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
  } = useForm<ProjectFormInputs>({
    resolver: yupResolver(validationSchema),
    defaultValues: {
      Title: "",
      Description: "",
      Tasks: [{ Body: "" }],
    },
    mode: "onSubmit",
    shouldUnregister: false,
  });

  const { fields, append } = useFieldArray({
    control,
    name: "Tasks",
  });

  const addTask = () => {
    append({ Body: "" });
  };

  const handleAddProject = async (data: ProjectFormInputs) => {
    projectsPostAPI(data)
      .then((res) => {
        console.log(res);
        if (res) {
          closeModal();
          toast.success("Project created!", {
            autoClose: 1000,
          });
          navigate(`/projects/${res.data.id}`);
        }
      })
      .catch((e) => {
        type FormField =
          | "Title"
          | "Description"
          | "Tasks"
          | `Tasks.${number}`
          | `Tasks.${number}.Body`;

        interface FormErrors {
          [key: string]: string[];
        }

        const errors: FormErrors = e.response?.data?.errors;

        if (errors) {
          Object.keys(errors).forEach((key) => {
            setError(key as FormField, {
              type: "manual",
              message: errors[key][0],
            });
          });
        }

        console.log(e.response.data.errors);
        toast.warning(e);
      });
  };

  return (
    <Modal isOpen={isOpen}>
      <h1 className="font-normal mb-16 text-2xl text-center">
        Let's Start Something New
      </h1>
      <form onSubmit={handleSubmit(handleAddProject)}>
        <div className="flex">
          <div className="flex-1 mr-4">
            <div className="mb-4">
              <label htmlFor="title" className="text-sm block mb-2">
                Title
              </label>
              <input
                type="text"
                id="title"
                className={
                  `border p-2 text-xs block w-full rounded ` +
                  (errors.Title ? "border-error" : "border-muted-light")
                }
                {...register("Title")}
              />
              {errors.Title && (
                <p className="text-error text-xs">{errors.Title.message}</p>
              )}
            </div>
            <div className="mb-4">
              <label htmlFor="description" className="text-sm block mb-2">
                Description
              </label>
              <textarea
                id="description"
                className={
                  `border p-2 text-xs block w-full rounded ` +
                  (errors.Description ? "border-error" : "border-muted-light")
                }
                rows={7}
                {...register("Description")}
              ></textarea>

              {errors.Description && (
                <p className="text-error text-xs">
                  {errors.Description.message}
                </p>
              )}
            </div>
          </div>

          <div className="flex-1 ml-4">
            <div className="mb-4">
              <label className="text-sm block mb-2">Need Some Tasks?</label>
              {fields.map((field, index) => (
                <div key={field.id} className="mb-2">
                  <Controller
                    name={`Tasks.${index}.Body`}
                    control={control}
                    defaultValue={field.Body || ""}
                    render={({ field }) => (
                      <input
                        type="text"
                        className={
                          `border p-2 text-xs block w-full rounded ` +
                          (errors.Tasks?.[index]?.Body
                            ? "border-error"
                            : "border-muted-light")
                        }
                        placeholder="Task"
                        {...field}
                      />
                    )}
                  />
                  {errors.Tasks?.[index]?.Body && (
                    <p className="text-error text-xs">
                      {errors.Tasks[index]?.Body?.message}
                    </p>
                  )}
                </div>
              ))}
            </div>

            <button
              type="button"
              className="inline-flex items-center text-xs"
              onClick={addTask}
            >
              <svg
                xmlns="http://www.w3.org/2000/svg"
                width="18"
                height="18"
                viewBox="0 0 18 18"
                className="mr-2"
              >
                <g fill="none" fillRule="evenodd" opacity=".307">
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
