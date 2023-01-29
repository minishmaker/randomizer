.thumb
ldr	r3,=#0x203F1F0
ldr	r1,[r3,#4]
cmp	r1,#0
bne	notfirst
ldr	r1,customRNG
str	r1,[r3]
mov	r1,#1
str	r1,[r3,#4]
notfirst:
ldr	r0,[r3]
ldr	r1,=#0x01010101
ldr	r2,=#0x31415927
mul	r0,r1
add	r0,r2
str	r0,[r3]
lsr	r0,#16
bx	lr
.align
.ltorg
customRNG:
