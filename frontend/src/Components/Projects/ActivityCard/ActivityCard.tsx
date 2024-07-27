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
const descriptionMessages: Record<string, (entityData?: string) => string> = {
  created: () => "You created the project",
  updated: () => "You updated the project",
  created_task: (body) => `You created ${body}`,
  completed_task: (body) => `You completed ${body}`,
  incompleted_task: (body) => `You incompleted ${body}`,
};

// Function to get message based on activity
const getActivityMessage = (
  activity: ProjectActivity | TaskActivity
): string => {
  const { subjectType, description, changes, entityData } = activity;
  if (subjectType === "Project") {
    if (description === "updated") {
      const changedKeys = Object.keys(changes.after);
      return changedKeys.length === 1
        ? `You updated the ${changedKeys[0]} of the project`
        : "You updated the project";
    }
    return descriptionMessages[description]?.() || "";
  }

  if (subjectType === "ProjectTask") {
    return descriptionMessages[description](entityData.body) || "";
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
