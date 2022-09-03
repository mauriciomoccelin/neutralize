CREATE TABLE
IF NOT EXISTS ToDo
(
    Id INTEGER PRIMARY KEY,
    Done BIT NOT NULL,
    Description VARCHAR
(255) NOT NULL
);

DELETE FROM ToDo;

INSERT INTO ToDo
VALUES
    (1, 1, 'Lorem ipsum 1'),
    (2, 0, 'Lorem ipsum 2'),
    (3, 0, 'Lorem ipsum 3'),
    (4, 1, 'Lorem ipsum 4'),
    (5, 1, 'Lorem ipsum 5'),
    (6, 1, 'Lorem ipsum 6'),
    (7, 1, 'Lorem ipsum 7'),
    (8, 1, 'Lorem ipsum 8'),
    (9, 1, 'Lorem ipsum 9'),
    (10, 1, 'Lorem ipsum 10'),
    (11, 1, 'Lorem ipsum 11');

