import React from "react";
import { ProjectGet } from "../../../Models/Project";
import moment from "moment";

interface Props {
  project: ProjectGet;
}

const ActivityCard = ({ project }: Props) => {
  const activityMap: {
    [key: string]: string | undefined;
  } = {
    created: "You created the project",
    updated: "You updated the project",
    created_task: "You created a task",
    completed_task: "You completed a task",
    incompleted_task: "You incompleted a task",
  };

  return (
    <div className="card mt-3">
      <ul className="text-xs">
        {project &&
          project?.activities.map((activity) => {
            return (
              <li key={activity.id} className="mb-1">
                {activityMap[activity.description]}
                &nbsp;
                <span className="text-grey">
                  {moment.utc(activity.createdAt).local().fromNow()}
                </span>
              </li>
            );
          })}
      </ul>
    </div>
  );
};

export default ActivityCard;
