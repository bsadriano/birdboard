export const gravatar_url: (email: string) => string = (email: string) =>
  `https://gravatar.com/avatar/${email}?s=60`;
