.thumb
ldrb	r0,[r4,#0x0A]

ldr	r0,=#0x3000BF0
ldrb	r1,[r0,#4]
ldrb	r2,[r0,#5]
ldr	r3,poin
loop:
ldrb	r0,[r3]
cmp	r0,#0
beq	noMatch
cmp	r0,r1
bne	next
ldrb	r0,[r3,#1]
cmp	r0,r2
beq	match
next:
add	r3,#3
b	loop

noMatch:
mov	r2,#0x40
b	end

match:
ldrb	r2,[r3,#2]

end:
sub	r2,#0x40
mov	r0,r4
mov	r1,#0xAD
ldr	r3,=#0x809FAC2
mov	lr,r3
mov	r3,#0
.short	0xF800

.align
.ltorg
poin:
