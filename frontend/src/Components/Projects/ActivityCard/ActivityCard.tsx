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

// Mapping of descriptions to messages
const descriptionMessages: Record<
  string,
  (userName: string, body?: string) => string
> = {
  created: (userName) => `${userName} created the project`,
  updated: (userName) => `${userName} created the project`,
  created_task: (userName, body) => `${userName} created ${body}`,
  completed_task: (userName, body) => `${userName} completed ${body}`,
  incompleted_task: (userName, body) => `${userName} incompleted ${body}`,
};

// Function to get message based on activity
const getActivityMessage = (
  activity: ProjectActivity | TaskActivity
): string => {
  const { subjectType, description, changes, entityData, user } = activity;
  if (subjectType === "Project") {
    if (description === "updated") {
      const changedKeys = Object.keys(changes.after);
      return changedKeys.length === 1
        ? `${user.userName} updated the ${changedKeys[0]} of the project`
        : `${user.userName} updated the project`;
    }

    return descriptionMessages[description]?.(user.userName) || "";
  }

  if (subjectType === "ProjectTask") {
    return (
      descriptionMessages[description](user.userName, entityData.body) || ""
    );
  }

  return ""; // Fallback if no match found
};

const ActivityCard = ({ project }: Props) => {
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
                    {getActivityMessage(activity)}
                    &nbsp;
                    <span className="text-grey">
                      {moment.utc(activity.createdAt).local().fromNow()}
                    </span>
                  </li>
                );
              } else if (activity.subjectType === "ProjectTask") {
                return (
                  <li key={activity.id} className="mb-1">
                    {getActivityMessage(activity)}
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
