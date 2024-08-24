import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import { toast } from "react-toastify";
import * as Yup from "yup";
import Agent from "../../../Api/Agent";
import { CreateProjectInvitationRequestDto } from "../../../Models/Project/ProjectInvitationRequestDto";

interface Props {
  projectId: number;
  onInviteUser: () => void;
}

const validation = Yup.object().shape({
  email: Yup.string().required("Email cannot be empty").email(),
});

const InviteCard = ({ projectId, onInviteUser }: Props) => {
  const {
    register,
    handleSubmit,
    reset,
    setError,
    formState: { errors },
  } = useForm<CreateProjectInvitationRequestDto>({
    resolver: yupResolver(validation),
  });

  async function handleInviteUser(body: CreateProjectInvitationRequestDto) {
    try {
      const data = await Agent.ProjectInvitation.create(projectId, body);
      if (data) {
        toast.success("User have been invited!");
        onInviteUser();
        reset();
      }
    } catch (err: any) {
      if (err?.data?.errors?.email) {
        setError("email", {
          type: "manual",
          message: err.data.errors.email,
        });
      }
    }
  }

  return (
    <div className="card flex flex-col mt-3">
      <h3 className="font-normal text-xl py-4 -ml-5 mb-3 border-l-4 border-accent pl-4">
        Invite a User
      </h3>

      <form onSubmit={handleSubmit(handleInviteUser)}>
        <div className="mb-3">
          <input
            required
            type="email"
            id="email"
            className={
              "bg-card text-default border rounded w-full py-2 px-3 " +
              (errors.email ? "border-red-400" : "border-grey")
            }
            placeholder="Email address"
            {...register("email")}
          />
          {errors.email && (
            <p className="text-error text-sm mt-2">{errors.email.message}</p>
          )}
        </div>

        <button type="submit" className="button">
          Invite
        </button>
      </form>
    </div>
  );
};

export default InviteCard;
