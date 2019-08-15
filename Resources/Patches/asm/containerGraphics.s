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
b	match
next:
add	r2,#4
b	loop

vanilla:
mov	r0,#0x62
match:
strb	r0,[r4,#0x0A]
mov	r0,#0x62
ldrb	r1,[r4,#0x18]
sub	r0,#0x66
and	r0,r1
ldr	r3,=#0x808DF93
bx	r3
.align
.ltorg
poin:
