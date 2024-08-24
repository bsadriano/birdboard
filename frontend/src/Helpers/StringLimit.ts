export const stringLimit = (
  value: string,
  limit: number,
  end: string = "..."
) => {
  if (value.trim().length <= limit) {
    return value;
  }

  return value.trim().slice(0, limit).trimEnd() + end;
};
