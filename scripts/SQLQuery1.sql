UPDATE dbo.preNode
SET value=LTRIM(RTRIM(value))


CREATE TABLE nodes
( id int IDENTITY(1,1) NOT NULL,
VALUE NVARCHAR(max)
)

INSERT INTO nodes
        ( VALUE )
SELECT DISTINCT value FROM dbo.preNode
ORDER BY value

UPDATE dbo.preNode
SET nodeID=nodes.id
FROM nodes
INNER JOIN dbo.preNode
ON nodes.VALUE=preNode.value


