import * as Yup from "yup";
import { yupResolver } from "@hookform/resolvers/yup";
import { useAuth } from "../../Context/useAuth";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
interface Props {}

type LoginFormInputs = {
  userName: string;
  password: string;
};

const validation = Yup.object().shape({
  userName: Yup.string().required("Username is required"),
  password: Yup.string().required("Password is required"),
});

const LoginPage = (props: Props) => {
  const { loginUser } = useAuth();
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginFormInputs>({ resolver: yupResolver(validation) });

  const handleLogin = (form: LoginFormInputs) => {
    loginUser(form.userName, form.password);
  };

  return (
    <div className="w-1/2 mx-auto bg-card py-12 px-16 rounded shadow text-default">
      <h1 className="font-normal mb-16 text-2xl text-center">Login</h1>

      <form onSubmit={handleSubmit(handleLogin)}>
        <div className="flex flex-col">
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
            Login
          </button>
          <p className="text-sm text-default font-light text-muted">
            Don’t have an account yet?{" "}
            <Link to="/register" className="font-medium hover:underline">
              Sign up
            </Link>
          </p>
        </footer>
      </form>
    </div>
  );
};

export default LoginPage;
