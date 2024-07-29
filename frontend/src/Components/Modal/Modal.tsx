import { Children } from "react";

interface Props {
  isOpen: boolean;
  onClose: () => void;
  children: React.ReactNode;
}

const Modal = ({ isOpen, onClose, children }: Props) => {
  return (
    <div
      className={`fixed inset-0 flex justify-center items-center transition-colors ${
        isOpen ? "visible bg-black/20" : "invisible"
      }`}
    >
      <div className="p-10 bg-card rounded-lg w-full max-w-xl max-h-screen overflow-auto flex flex-col">
        {children}
      </div>
    </div>
  );
};

export default Modal;
