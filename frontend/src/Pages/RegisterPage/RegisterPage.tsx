import { yupResolver } from "@hookform/resolvers/yup";
import { useState } from "react";
import { useForm } from "react-hook-form";
import { Link } from "react-router-dom";
import * as Yup from "yup";
import { useAuth } from "../../Context/useAuth";
import { RegisterRequestDto } from "../../Models/Register/RegisterRequestDto";

interface Props {}

const validation = Yup.object().shape({
  email: Yup.string().email().required("Email is required"),
  userName: Yup.string().required("Username is required"),
  password: Yup.string().required("Password is required"),
});

const RegisterPage = (props: Props) => {
  const { registerUser } = useAuth();
  const [errors, setErrors] = useState<any>([]);
  const { register, handleSubmit } = useForm<RegisterRequestDto>({
    resolver: yupResolver(validation),
  });

  const handleRegister = async (form: RegisterRequestDto) => {
    try {
      await registerUser(form);
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
              className={`input bg-transparent border border-muted-light rounded p-2 text-xs text-default w-full `}
              {...register("email")}
            />
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
