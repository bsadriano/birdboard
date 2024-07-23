import React from "react";
import * as Yup from "yup";
import { ProjectGet } from "../../../Models/Project";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { projectPatchAPI } from "../../../Services/ProjectService";
import { toast } from "react-toastify";

interface Props {
  project: ProjectGet;
}

type UpdateNotesFormInput = {
  notes?: string;
};

const validation = Yup.object().shape({
  notes: Yup.string()
    .min(3, "Notes cannot be less than 3 characters")
    .max(100, "Notes cannot be over 50 characters"),
});

const GeneralNotes = ({ project }: Props) => {
  console.log("project: ", project);
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<UpdateNotesFormInput>({
    defaultValues: { notes: project?.notes },
    resolver: yupResolver(validation),
  });

  const handleUpdateNotes = (data: UpdateNotesFormInput) => {
    projectPatchAPI(project!.id, data)
      .then((res) => {
        if (res) {
          toast.success("General notes updated");
        }
      })
      .catch((e) => {
        toast.warning("Could not update project!");
      });
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
          <p className="text-red-400 text-sm">{errors.notes.message}</p>
        )}
      </div>
      <button type="submit" className="button">
        Save
      </button>
    </form>
  );
};

export default GeneralNotes;
