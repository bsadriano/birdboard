import { yupResolver } from "@hookform/resolvers/yup";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import * as Yup from "yup";
import { useAuth } from "../../Context/useAuth";
import { LoginRequestDto } from "../../Models/Login/LoginRequestDto";

interface Props {}

const validation = Yup.object().shape({
  userName: Yup.string().required("Username is required"),
  password: Yup.string().required("Password is required"),
});

const LoginPage = (props: Props) => {
  const { loginUser } = useAuth();
  const [errors, setErrors] = useState<any>([]);
  const { register, handleSubmit } = useForm<LoginRequestDto>({
    resolver: yupResolver(validation),
  });

  const handleLogin = async (form: LoginRequestDto) => {
    try {
      await loginUser(form);
    } catch (err: any) {
      if (err.data.errors) {
        setErrors(
          Object.keys(err.data.errors).map((key) => {
            return errors[key][0];
          })
        );
      } else if (err.data && Array.isArray(err.data)) {
        setErrors(err.data.map((error: any) => error.description));
      } else if (err.data) {
        setErrors([err.data]);
      }
    }
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
              className={`input bg-transparent border border-muted-light rounded p-2 text-xs text-default w-full `}
              {...register("userName")}
            />
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
              className={`input bg-transparent border border-muted-light rounded p-2 text-xs text-default w-full`}
              {...register("password")}
            />
          </div>

          {errors &&
            errors.map((error: string) => (
              <p key={error} className="text-error text-xs mb-4">
                {error}
              </p>
            ))}
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
