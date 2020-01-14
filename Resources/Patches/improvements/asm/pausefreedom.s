.thumb
@shrinking check
ldr	r1,=#0x300116C
ldrb	r3,[r1]
cmp	r3,#0x15
bne	noshrink
ldrb	r1,[r1,#1]
cmp	r1,#0x4
bhs	false
noshrink:

@fade check
ldr	r1,=#0x3000FD0
ldrb	r1,[r1]
cmp	r1,#0
bne	false

cmp	r0,#0xA
bhi	true

vanilla:
lsl	r0,#2
ldr	r1,=#0x8070084
ldr	r1,[r1]
add	r0,r1
ldr	r0,[r0]
mov	pc,r0

true:
ldr	r3,=#0x80700B9
bx	r3

false:
mov	r0,#0x4
b	vanilla
