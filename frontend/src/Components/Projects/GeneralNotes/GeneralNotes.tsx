import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as Yup from "yup";
import Agent from "../../../Api/Agent";
import { ProjectResponseDto } from "../../../Models/Project/ProjectResponseDto";

interface Props {
  project: ProjectResponseDto;
}

type UpdateNotesFormInput = {
  notes?: string;
};

const validation = Yup.object().shape(
  {
    notes: Yup.string().when("notes", (value) => {
      if (value[0]) {
        return Yup.string()
          .min(3, "Notes cannot be less than 3 characters")
          .max(100, "Notes cannot be over 50 characters");
      } else {
        return Yup.string()
          .transform((value, originalValue) => {
            // Convert empty values to null
            if (!value) {
              return null;
            }
            return originalValue;
          })
          .nullable()
          .optional();
      }
    }),
  },
  [["notes", "notes"]]
);

const GeneralNotes = ({ project }: Props) => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UpdateNotesFormInput>({
    defaultValues: { notes: project?.notes ?? "" },
    // defaultValues: { notes: "" },
    resolver: yupResolver(validation),
  });

  const handleUpdateNotes = async (body: UpdateNotesFormInput) => {
    try {
      const data = await Agent.Project.update(project!.id, body);
      if (data) {
        toast.success("General notes updated");
      }
    } catch (error) {
      toast.warning("Could not update project!");
    }
  };
  return (
    <form onSubmit={handleSubmit(handleUpdateNotes)}>
      <div className="mb-4">
        <textarea
          className="card w-full"
          style={{ minHeight: "200px" }}
          placeholder="Anything special that you want to make a note of?"
          {...register("notes")}
        ></textarea>
        {errors.notes && (
          <p className="text-error text-sm mt-2">{errors.notes.message}</p>
        )}
      </div>
      <button type="submit" className="button">
        Save
      </button>
    </form>
  );
};

export default GeneralNotes;
