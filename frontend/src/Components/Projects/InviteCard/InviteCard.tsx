import { yupResolver } from "@hookform/resolvers/yup";
import { useForm } from "react-hook-form";
import * as Yup from "yup";
import { projectInvitationsPostApi } from "../../../Services/ProjectInvitationService";
import { toast } from "react-toastify";

interface Props {
  projectId: number;
  onInviteUser: () => void;
}

type InviteFormInputs = {
  email?: string;
};

const validation = Yup.object().shape({
  email: Yup.string().email(),
});

const InviteCard = ({ projectId, onInviteUser }: Props) => {
  const {
    register,
    handleSubmit,
    reset,
    setError,
    formState: { errors },
  } = useForm<InviteFormInputs>({ resolver: yupResolver(validation) });

  function handleInviteUser({ email }: InviteFormInputs): void {
    projectInvitationsPostApi(projectId, email!)
      .then((res) => {
        toast.success("User have been invited!");
        onInviteUser();
        reset();
      })
      .catch((error) => {
        var err = error.response;

        if (err?.data?.errors?.email) {
          setError("email", {
            type: "manual",
            message: err.data.errors.email,
          });
        }
      });
  }

  return (
    <div className="card flex flex-col mt-3">
      <h3 className="font-normal text-xl py-4 -ml-5 mb-3 border-l-4 border-blue-light pl-4">
        Invite a User
      </h3>

      <form onSubmit={handleSubmit(handleInviteUser)}>
        <div className="mb-3">
          <input
            type="email"
            id="email"
            className="border border-grey rounded w-full py-2 px-3"
            placeholder="Email address"
            {...register("email")}
          />
          {errors.email && (
            <p className="text-red-400 text-sm">{errors.email.message}</p>
          )}
        </div>

        <button type="submit" className="button">
          Inivte
        </button>
      </form>
    </div>
  );
};

export default InviteCard;
