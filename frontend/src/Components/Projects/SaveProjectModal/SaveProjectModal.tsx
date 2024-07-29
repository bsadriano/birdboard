import { useState } from "react";
import Modal from "../../Modal/Modal";

interface Props {
  isOpen: boolean;
  closeModal: () => void;
}

const SaveProjectModal = ({ isOpen, closeModal }: Props) => {
  return (
    <Modal isOpen={isOpen} onClose={closeModal}>
      <h1 className="font-normal mb-16 text-2xl text-center">
        Let's Start Something New
      </h1>

      <div className="flex">
        <div className="flex-1 mr-4">
          <div className="mb-4">
            <label htmlFor="title" className="text-sm block mb-2">
              Title
            </label>
            <input
              type="text"
              id="title"
              className="border border-muted-light p-2 text-xs block w-full rounded"
            />
          </div>
          <div className="mb-4">
            <label htmlFor="description" className="text-sm block mb-2">
              Description
            </label>
            <textarea
              id="description"
              className="border border-muted-light p-2 text-xs block w-full rounded"
              rows={7}
            ></textarea>
          </div>
        </div>

        <div className="flex-1 ml-4">
          <div className="mb-4">
            <label className="text-sm block mb-2">Need Some Tasks?</label>
            <input
              type="text"
              className="border border-muted-light p-2 text-xs block w-full rounded"
              placeholder="Task 1"
            />
          </div>

          <button className="inline-flex items-center text-xs">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              width="18"
              height="18"
              viewBox="0 0 18 18"
              className="mr-2"
            >
              <g fill="none" fill-rule="evenodd" opacity=".307">
                <path
                  stroke="#000"
                  stroke-opacity=".012"
                  stroke-width="0"
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
        <button className="button is-outlined mr-4" onClick={closeModal}>
          Cancel
        </button>
        <button className="button">Create Project</button>
      </footer>
    </Modal>
  );
};

export default SaveProjectModal;
