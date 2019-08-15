.thumb
@get area and room id
ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#5]
ldrb	r0,[r0,#4]

@find the room in the list
ldr	r2,poin
loop:
ldr	r3,[r2]
cmp	r3,#0
beq	vanilla
ldrb	r3,[r2]
cmp	r3,r0
bne	next
ldrb	r3,[r2,#1]
cmp	r3,r1
bne	next
ldrb	r0,[r2,#2]
ldrb	r1,[r2,#3]
b	match
next:
add	r2,#4
b	loop

vanilla:
mov	r0,#0x62
mov	r1,#0
match:
mov	r2,#0
ldr	r3,=#0x80A73F8
mov	lr,r3
.short	0xF800
ldr	r3,=#0x808E067
bx	r3
.align
.ltorg
poin:
