import * as Yup from "yup";
import { useForm } from "react-hook-form";
import { useAuth } from "../../Context/useAuth";
import { yupResolver } from "@hookform/resolvers/yup";
import { Link } from "react-router-dom";

interface Props {}

type RegisterFormInputs = {
  email: string;
  userName: string;
  password: string;
};

const validation = Yup.object().shape({
  email: Yup.string().email().required("Email is required"),
  userName: Yup.string().required("Username is required"),
  password: Yup.string().required("Password is required"),
});

const RegisterPage = (props: Props) => {
  const { registerUser } = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterFormInputs>({ resolver: yupResolver(validation) });

  const handleRegister = (form: RegisterFormInputs) => {
    registerUser(form.email, form.userName, form.password);
  };

  return (
    <div className="w-1/2 mx-auto bg-card py-12 px-16 rounded shadow">
      <h1 className="font-normal mb-16 text-2xl text-center text-default">
        Register
      </h1>

      <form onSubmit={handleSubmit(handleRegister)}>
        <div className="flex flex-col">
          <div className="mb-4">
            <label htmlFor="email" className="text-sm text-default block mb-2">
              Email
            </label>

            <input
              id="email"
              type="email"
              required
              className={`input bg-transparent border ${
                errors.email ? "border-error" : "border-muted-light"
              } rounded p-2 text-xs text-default w-full `}
              {...register("email")}
            />

            {errors.email && (
              <p className="text-error text-xs">{errors.email.message}</p>
            )}
          </div>

          <div className="mb-4">
            <label
              htmlFor="userName"
              className="text-sm text-default block mb-2"
            >
              Username
            </label>

            <input
              id="userName"
              required
              className={`input bg-transparent border ${
                errors.userName ? "border-error" : "border-muted-light"
              } rounded p-2 text-xs text-default w-full `}
              {...register("userName")}
            />

            {errors.userName && (
              <p className="text-error text-xs">{errors.userName.message}</p>
            )}
          </div>

          <div className="mb-4">
            <label
              htmlFor="password"
              className="text-sm text-default block mb-2"
            >
              Password
            </label>
            <input
              id="password"
              type="password"
              required
              className={`input bg-transparent border ${
                errors.password ? "border-error" : "border-muted-light"
              } rounded p-2 text-xs text-default w-full`}
              {...register("password")}
            />
            {errors.password && (
              <p className="text-error text-xs">{errors.password.message}</p>
            )}
          </div>
        </div>

        <footer className="flex flex-col items-center">
          <button type="submit" className="button mr-4 mb-4">
            Sign Up
          </button>
          <p className="text-sm font-light text-muted">
            Already have an account?&nbsp;
            <Link to="/login" className="font-medium hover:underline">
              Login
            </Link>
          </p>
        </footer>
      </form>
    </div>
  );
};

export default RegisterPage;
