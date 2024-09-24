// JavaScript source code
print("Started Adding the Users.");
db = db.getSiblingDB("${PAYMENT_MONGO_ADMIN_DB}");
db.createUser({
    user: "${PAYMENT_MONGO_USER}",
    pwd: "${PAYMENT_MONGO_PASSWORD}",
    roles: [{ role: "readWrite", db: "${PAYMENT_MONGO_ADMIN_DB}" }],
});
db2 = db.getSiblingDB("PAYMENT_system");
db2.createUser({
    user: "${PAYMENT_MONGO_USER}",
    pwd: "${PAYMENT_MONGO_PASSWORD}",
    roles: [{ role: "readWrite", db: "${PAYMENT_MONGO_DATABASE}" }],
});
print("End Adding the User Roles.");