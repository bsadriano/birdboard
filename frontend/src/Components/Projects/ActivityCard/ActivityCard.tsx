import React from "react";
import {
  ProjectActivity,
  ProjectGet,
  TaskActivity,
} from "../../../Models/Project";
import moment from "moment";

interface Props {
  project: ProjectGet;
}

type ActivityMap = {
  created: string;
  updated: string;
  created_task: (task: string) => string;
  completed_task: (task: string) => string;
  incompleted_task: (task: string) => string;
};

export type ActivityMapKey = keyof ActivityMap;

const ActivityCard = ({ project }: Props) => {
  const activityMap: ActivityMap = {
    created: "You created the project",
    updated: "You updated the project",
    created_task: (task) => `You created ${task}`,
    completed_task: (task) => `You completed ${task}`,
    incompleted_task: (task) => `You incompleted ${task}`,
  };

  return (
    <div className="card mt-3">
      <ul className="text-xs">
        {project &&
          Array.isArray(project?.activities) &&
          project?.activities.map(
            (activity: ProjectActivity | TaskActivity) => {
              if (activity.subjectType === "Project") {
                return (
                  <li key={activity.id} className="mb-1">
                    {activity.description === "created" &&
                      "You created the project"}
                    {activity.description === "updated" &&
                      "You updated the project"}
                    &nbsp;
                    <span className="text-grey">
                      {moment.utc(activity.createdAt).local().fromNow()}
                    </span>
                  </li>
                );
              } else if (activity.subjectType === "ProjectTask") {
                return (
                  <li key={activity.id} className="mb-1">
                    {activity.description === "updated" &&
                      `You updated ${activity.entityData.body}`}
                    {activity.description === "created_task" &&
                      `You created ${activity.entityData.body}`}
                    {activity.description === "completed_task" &&
                      `You completed ${activity.entityData.body}`}
                    {activity.description === "incompleted_task" &&
                      `You incompleted ${activity.entityData.body}`}
                    &nbsp;
                    <span className="text-grey">
                      {moment.utc(activity.createdAt).local().fromNow()}
                    </span>
                  </li>
                );
              }
            }
          )}
      </ul>
    </div>
  );
};

export default ActivityCard;
